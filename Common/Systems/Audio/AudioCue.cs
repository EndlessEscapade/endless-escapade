using System.Runtime.InteropServices;
using Microsoft.Xna.Framework;
using uint8_t = System.Byte;
using int8_t = System.SByte;
using int16_t = System.Int16;
using uint16_t = System.UInt16;
using int32_t = System.Int32;
using uint32_t = System.UInt32;
using int64_t = System.Int64;
using uint64_t = System.UInt64;
using FAudioThread = System.IntPtr;
using FAudioMutex = System.IntPtr;
using FAudioDecodeCallback = System.IntPtr;
using FAudioResampleCallback = System.IntPtr;
using FAudioMixCallback = System.IntPtr;
using FAudioFilterState = Microsoft.Xna.Framework.Vector4;

/* FAudio - XAudio Reimplementation for FNA
*
* Copyright (c) 2011-2023 Ethan Lee, Luigi Auriemma, and the MonoGame Team
*
* This software is provided 'as-is', without any express or implied warranty.
* In no event will the authors be held liable for any damages arising from
* the use of this software.
*
* Permission is granted to anyone to use this software for any purpose,
* including commercial applications, and to alter it and redistribute it
* freely, subject to the following restrictions:
*
* 1. The origin of this software must not be misrepresented; you must not
* claim that you wrote the original software. If you use this software in a
* product, an acknowledgment in the product documentation would be
* appreciated but is not required.
*
* 2. Altered source versions must be plainly marked as such, and must not be
* misrepresented as being the original software.
*
* 3. This notice may not be removed or altered from any source distribution.
*
* Ethan "flibitijibibo" Lee <flibitijibibo@flibitijibibo.com>
*
*/

// These bindings were made by EndlessEscapade, copying and modifying the original C impl

// SOME TYPES DO NOT FULLY MATCH THE C++ IMPL
// IF YOU WANNA MAKE SURE THEY DO CHECK
// FACT.c https://github.com/FNA-XNA/FAudio/blob/29a7d3a726383a3907baf4930d2c4d4da773b023/src/FACT.c
// FAudio.c https://github.com/FNA-XNA/FAudio/blob/29a7d3a726383a3907baf4930d2c4d4da773b023/include/FAudio.h#L1293
// FAudio_internal.h https://github.com/FNA-XNA/FAudio/blob/29a7d3a726383a3907baf4930d2c4d4da773b023/src/FAudio_internal.h
// and other files
// Make public the types that are going to be used

namespace FAudioINTERNAL;

#region FAudio.h

//typedef struct FAudio FAudio;
//typedef struct FAudioVoice FAudioVoice;
using FAudioSourceVoice = FAudioVoice;
using FAudioSubmixVoice = FAudioVoice;
using FAudioMasteringVoice = FAudioVoice;

#endregion FAudio.h

#region FAudio_internal.h

using FAudioThread = FAudioMutex; //typedef void* FAudioThread;
using FAudioMutex = FAudioThread; //typedef void* FAudioMutex;

#endregion

internal enum FAPOBufferFlags
{
    FAPO_BUFFER_SILENT,
    FAPO_BUFFER_VALID
}

internal enum FAudioFilterType
{
    FAudioLowPassFilter,
    FAudioBandPassFilter,
    FAudioHighPassFilter,
    FAudioNotchFilter
}

internal struct FAudioFilterParametersEXT
{
    private FAudioFilterType Type;
    private float Frequency; /* [0, FAUDIO_MAX_FILTER_FREQUENCY] */
    private float OneOverQ; /* [0, FAUDIO_MAX_FILTER_ONEOVERQ] */
    private float WetDryMix; /* [0, 1] */
}

//typedef struct FAudioEngineCallback FAudioEngineCallback;
//typedef struct FAudioVoiceCallback FAudioVoiceCallback;

#region FAudio.h

internal unsafe struct FAudioGUID
{
    private uint Data1;
    private ushort Data2;
    private ushort Data3;
    private fixed byte Data4[8];
}

internal unsafe struct FAudioSendDescriptor
{
    private uint Flags; /* 0 or FAUDIO_SEND_USEFILTER */
    private FAudioVoice* pOutputVoice;
}

internal unsafe struct FAudioVoiceSends
{
    private uint SendCount;
    private FAudioSendDescriptor* pSends;
}

internal unsafe struct FAudioEffectDescriptor
{
    /*FAPO*/
    private void* pEffect;
    private int InitialState; /* 1 - Enabled, 0 - Disabled */
    private uint OutputChannels;
}

internal unsafe struct FAudioBuffer
{
    /* Either 0 or FAUDIO_END_OF_STREAM */
    private uint Flags;

    /* Pointer to wave data, memory block size.
	 * Note that pAudioData is not copied; FAudio reads directly from your
	 * pointer! This pointer must be valid until FAudio has finished using
	 * it, at which point an OnBufferEnd callback will be generated.
	 */
    private uint AudioBytes;

    private byte* pAudioData; // readonly

    /* Play region, in sample frames. */
    private uint PlayBegin;

    private uint PlayLength;

    /* Loop region, in sample frames.
	 * This can be used to loop a subregion of the wave instead of looping
	 * the whole thing, i.e. if you have an intro/outro you can set these
	 * to loop the middle sections instead. If you don't need this, set both
	 * values to 0.
	 */
    private uint LoopBegin;

    private uint LoopLength;

    /* [0, FAUDIO_LOOP_INFINITE] */
    private uint LoopCount;

    /* This is sent to callbacks as pBufferContext */
    private void* pContext;
}

internal unsafe struct FAudioBufferWMA
{
    private uint* pDecodedPacketCumulativeBytes;
    private uint PacketCount;
}

internal struct FAudioWaveFormatEx
{
    private ushort wFormatTag;
    private ushort nChannels;
    private uint nSamplesPerSec;
    private uint nAvgBytesPerSec;
    private ushort nBlockAlign;
    private ushort wBitsPerSample;
    private ushort cbSize;
}

internal struct FAudioWaveFormatExtensible
{
    private FAudioWaveFormatEx Format;

    //union
    [StructLayout(LayoutKind.Explicit)]
    private struct _Samples
    {
        [FieldOffset(0)] private readonly ushort wValidBitsPerSample;
        [FieldOffset(0)] private readonly ushort wSamplesPerBlock;
        [FieldOffset(0)] private readonly ushort wReserved;
    }

    private _Samples Samples;
    private uint dwChannelMask;
    private FAudioGUID SubFormat;
}

#endregion

#region FAudio_internal.h

//typedef int32_t (FAUDIOCALL * FAudioThreadFunc)(void* data);
internal enum FAudioThreadPriority
{
    FAUDIO_THREAD_PRIORITY_LOW,
    FAUDIO_THREAD_PRIORITY_NORMAL,
    FAUDIO_THREAD_PRIORITY_HIGH
}

/* Linked Lists */
internal unsafe struct LinkedList
{
    private void* entry;
    private LinkedList* next;
}

/* Internal FAudio Types */

internal enum FAudioVoiceType
{
    FAUDIO_VOICE_SOURCE,
    FAUDIO_VOICE_SUBMIX,
    FAUDIO_VOICE_MASTER
}

internal unsafe struct FAudioBufferEntry
{
    private FAudioBuffer buffer;
    private FAudioBufferWMA bufferWMA;
    private FAudioBufferEntry* next;
}

//unsafe delegate void FAudioDecodeCallback_(
//	FAudioVoice *voice,
//	FAudioBuffer *buffer,	/* Buffer to decode */
//	float *decodeCache,	/* Decode into here */
//	uint32_t samples	/* Samples to decode */
//);

//unsafe delegate void FAudioResampleCallback_(
//	float *restrict dCache,
//	float *restrict resampleCache,
//	uint64_t *resampleOffset,
//	uint64_t resampleStep,
//	uint64_t toResample,
//	uint8_t channels
//);

//unsafe delegate void FAudioMixCallback_(
//	uint32_t toMix,
//	uint32_t srcChans,
//	uint32_t dstChans,
//	float *restrict srcData,
//	float *restrict dstData,
//	float *restrict coefficients
//);
/* Operation Sets, original implementation by Tyler Glaiel */
/* Public FAudio Types */

internal unsafe struct FAudio
{
    private byte version;
    private byte active;
    private uint refcount;
    private uint initFlags;
    private uint updateSize;
    private FAudioMasteringVoice* master;
    private LinkedList* sources;
    private LinkedList* submixes;
    private LinkedList* callbacks;
    private FAudioMutex sourceLock;
    private FAudioMutex submixLock;
    private FAudioMutex callbackLock;
    private FAudioMutex operationLock;
    private FAudioWaveFormatExtensible mixFormat;

    /*FAudio_OPERATIONSET_Operation*/
    private void* queuedOperations;

    /*FAudio_OPERATIONSET_Operation*/
    private void* committedOperations;

    /* Used to prevent destroying an active voice */
    private FAudioSourceVoice* processingSource;

    /* Temp storage for processing, interleaved PCM32F */
    // #define EXTRA_DECODE_PADDING 2
    private uint decodeSamples;
    private uint resampleSamples;
    private uint effectChainSamples;
    private float* decodeCache;
    private float* resampleCache;
    private float* effectChainCache;

    /* Allocator callbacks */
    /*FAudioMallocFunc*/
    private void* pMalloc;

    /*FAudioFreeFunc*/
    private void* pFree;

    /*FAudioReallocFunc*/
    private void* pRealloc;

    /* EngineProcedureEXT */
    private void* clientEngineUser;

    /*FAudioEngineProcedureEXT*/
    private void* pClientEngineProc;

    // #ifndef FAUDIO_DISABLE_DEBUGCONFIGURATION
    /* Debug Information */
    // FAudioDebugConfiguration debug;
    // #endif /* FAUDIO_DISABLE_DEBUGCONFIGURATION */

    /* Platform opaque pointer */
    private void* platform;
}

internal unsafe struct FAudioVoice
{
    private FAudio* audio;
    private uint flags;
    private FAudioVoiceType type;

    private FAudioVoiceSends sends;
    private float** sendCoefficients;
    private float** mixCoefficients;

    private FAudioMixCallback* sendMix;

    /*FAudioFilterParametersEXT*/
    private void* sendFilter;
    private FAudioFilterState** sendFilterState;

    private struct _effects
    {
        private FAPOBufferFlags state;
        private uint count;
        private FAudioEffectDescriptor* desc;
        private void** parameters;
        private uint* parameterSizes;
        private byte* parameterUpdates;
        private byte* inPlaceProcessing;
    }

    private _effects effects;
    private FAudioFilterParametersEXT filter;
    private FAudioFilterState* filterState;
    private FAudioMutex sendLock;
    private FAudioMutex effectLock;
    private FAudioMutex filterLock;

    private float volume;
    private float* channelVolume;
    private uint outputChannels;
    private FAudioMutex volumeLock;

    //FAUDIONAMELESS union
    [StructLayout(LayoutKind.Explicit)]
    private struct NAMELESSUNION0
    {
        private struct _src
        {
            /* Sample storage */
            private uint decodeSamples;
            private uint resampleSamples;

            /* Resampler */
            private float resampleFreq;
            private ulong resampleStep;
            private ulong resampleOffset;
            private ulong curBufferOffsetDec;
            private uint curBufferOffset;

            /* WMA decoding */
            //#ifdef HAVE_WMADEC
            /*FAudioWMADEC*/
            private void* wmadec;
            //#endif /* HAVE_WMADEC*/

            /* Read-only */
            private float maxFreqRatio;
            private FAudioWaveFormatEx* format;
            private FAudioDecodeCallback decode;

            private FAudioResampleCallback resample;

            /*FAudioVoiceCallback*/
            private void* callback;

            /* Dynamic */
            private byte active;
            private float freqRatio;
            private byte newBuffer;
            private ulong totalSamples;
            private FAudioBufferEntry* bufferList;
            private FAudioBufferEntry* flushList;
            private FAudioMutex bufferLock;
        }

        [FieldOffset(0)] private readonly _src src;

        private struct _mix
        {
            /* Sample storage */
            private uint inputSamples;
            private uint outputSamples;
            private float* inputCache;
            private ulong resampleStep;
            private FAudioResampleCallback resample;

            /* Read-only */
            private uint inputChannels;
            private uint inputSampleRate;
            private uint processingStage;
        }

        [FieldOffset(0)] private readonly _mix mix;

        private struct _master
        {
            /* Output stream, allocated by Platform */
            private float* output;

            /* Needed when inputChannels != outputChannels */
            private float* effectCache;

            /* Read-only */
            private uint inputChannels;
            private uint inputSampleRate;
        }

        [FieldOffset(0)] private readonly _master master;
    }
}

#endregion // FAudio_internal.h

/* FAudio - XAudio Reimplementation for FNA
 *
 * Copyright (c) 2011-2023 Ethan Lee, Luigi Auriemma, and the MonoGame Team
 *
 * This software is provided 'as-is', without any express or implied warranty.
 * In no event will the authors be held liable for any damages arising from
 * the use of this software.
 *
 * Permission is granted to anyone to use this software for any purpose,
 * including commercial applications, and to alter it and redistribute it
 * freely, subject to the following restrictions:
 *
 * 1. The origin of this software must not be misrepresented; you must not
 * claim that you wrote the original software. If you use this software in a
 * product, an acknowledgment in the product documentation would be
 * appreciated but is not required.
 *
 * 2. Altered source versions must be plainly marked as such, and must not be
 * misrepresented as being the original software.
 *
 * 3. This notice may not be removed or altered from any source distribution.
 *
 * Ethan "flibitijibibo" Lee <flibitijibibo@flibitijibibo.com>
 *
 */
/* Internal AudioEngine Types */

internal struct FACTAudioCategory
{
    private byte instanceLimit;
    private ushort fadeInMS;
    private ushort fadeOutMS;
    private byte maxInstanceBehavior;
    private short parentCategory;
    private float volume;
    private byte visibility;

    private byte instanceCount;
    private float currentVolume;
}

internal struct FACTVariable
{
    private byte accessibility;
    private float initialValue;
    private float minValue;
    private float maxValue;
}

internal struct FACTRPCPoint
{
    private float x;
    private float y;
    private byte type;
}

internal enum FACTRPCParameter
{
    RPC_PARAMETER_VOLUME,
    RPC_PARAMETER_PITCH,
    RPC_PARAMETER_REVERBSEND,
    RPC_PARAMETER_FILTERFREQUENCY,
    RPC_PARAMETER_FILTERQFACTOR,
    RPC_PARAMETER_COUNT /* If >=, DSP Parameter! */
}

internal unsafe struct FACTRPC
{
    private ushort variable;
    private byte pointCount;
    private ushort parameter;
    private FACTRPCPoint* points;
}

internal struct FACTDSPParameter
{
    private byte type;
    private float value;
    private float minVal;
    private float maxVal;
    private ushort unknown;
}

internal unsafe struct FACTDSPPreset
{
    private byte accessibility;
    private ushort parameterCount;
    private FACTDSPParameter* parameters;
}

internal enum FACTNoticationsFlags
{
    NOTIFY_CUEPREPARED = 0x00000001,
    NOTIFY_CUEPLAY = 0x00000002,
    NOTIFY_CUESTOP = 0x00000004,
    NOTIFY_CUEDESTROY = 0x00000008,
    NOTIFY_MARKER = 0x00000010,
    NOTIFY_SOUNDBANKDESTROY = 0x00000020,
    NOTIFY_WAVEBANKDESTROY = 0x00000040,
    NOTIFY_LOCALVARIABLECHANGED = 0x00000080,
    NOTIFY_GLOBALVARIABLECHANGED = 0x00000100,
    NOTIFY_GUICONNECTED = 0x00000200,
    NOTIFY_GUIDISCONNECTED = 0x00000400,
    NOTIFY_WAVEPREPARED = 0x00000800,
    NOTIFY_WAVEPLAY = 0x00001000,
    NOTIFY_WAVESTOP = 0x00002000,
    NOTIFY_WAVELOOPED = 0x00004000,
    NOTIFY_WAVEDESTROY = 0x00008000,
    NOTIFY_WAVEBANKPREPARED = 0x00010000,
    NOTIFY_WAVEBANKSTREAMING_INVALIDCONTENT = 0x00020000
}

/* Internal SoundBank Types */

internal enum FACTEventType
{
    FACTEVENT_STOP = 0,
    FACTEVENT_PLAYWAVE = 1,
    FACTEVENT_PLAYWAVETRACKVARIATION = 3,
    FACTEVENT_PLAYWAVEEFFECTVARIATION = 4,
    FACTEVENT_PLAYWAVETRACKEFFECTVARIATION = 6,
    FACTEVENT_PITCH = 7,
    FACTEVENT_VOLUME = 8,
    FACTEVENT_MARKER = 9,
    FACTEVENT_PITCHREPEATING = 16,
    FACTEVENT_VOLUMEREPEATING = 17,
    FACTEVENT_MARKERREPEATING = 18
}

internal unsafe struct FACTEvent
{
    private ushort type;
    private ushort timestamp;

    private ushort randomOffset;

    //FAUDIONAMELESS union
    [StructLayout(LayoutKind.Explicit)]
    private struct NAMELESSUNION0
    {
        /* Play Wave Event */
        private struct _wave
        {
            private byte flags;
            private byte loopCount;
            private ushort position;
            private ushort angle;

            /* Track Variation */
            private byte isComplex;

            //FAUDIONAMELESS union
            [StructLayout(LayoutKind.Explicit)]
            private struct NAMELESSUNION1
            {
                private struct _simple
                {
                    private ushort track;
                    private byte wavebank;
                }

                [FieldOffset(0)] private readonly _simple simple;

                private struct _complex
                {
                    private ushort variation;
                    private ushort trackCount;
                    private ushort* tracks;
                    private byte* wavebanks;
                    private byte* weights;
                }

                [FieldOffset(0)] private readonly _complex complex;
            }

            /* Effect Variation */
            private short minPitch;
            private short maxPitch;
            private float minVolume;
            private float maxVolume;
            private float minFrequency;
            private float maxFrequency;
            private float minQFactor;
            private float maxQFactor;
            private ushort variationFlags;
        }

        [FieldOffset(0)] private readonly _wave wave;

        /* Set Pitch/Volume Event */
        private struct _value
        {
            private byte settings;
            private ushort repeats;

            private ushort frequency;

            //FAUDIONAMELESS union
            [StructLayout(LayoutKind.Explicit)]
            private struct NAMELESSUNION3
            {
                private struct _ramp
                {
                    private float initialValue;
                    private float initialSlope;
                    private float slopeDelta;
                    private ushort duration;
                }

                [FieldOffset(0)] private readonly _ramp ramp;

                private struct _equation
                {
                    private byte flags;
                    private float value1;
                    private float value2;
                }

                [FieldOffset(0)] private readonly _equation equation;
            }
        }

        [FieldOffset(0)] private readonly _value value;

        /* Stop Event */
        private struct _stop
        {
            private byte flags;
        }

        [FieldOffset(0)] private readonly _stop stop;

        /* Marker Event */
        private struct _marker
        {
            private uint marker;
            private ushort repeats;
            private ushort frequency;
        }

        [FieldOffset(0)] private readonly _marker marker;
    }
}

internal unsafe struct FACTTrack
{
    public uint code;

    public float volume;
    public byte filter;
    public byte qfactor;
    public ushort frequency;

    public byte rpcCodeCount;
    public uint* rpcCodes;

    public byte eventCount;
    public FACTEvent* events;
}

internal unsafe struct FACTSound
{
    public byte flags;
    public ushort category;
    public float volume;
    public short pitch;
    public byte priority;

    public byte trackCount;
    public byte rpcCodeCount;
    public byte dspCodeCount;

    public FACTTrack* tracks;
    public uint* rpcCodes;
    public uint* dspCodes;
}

internal struct FACTCueData
{
    private byte flags;
    private uint sbCode;
    private uint transitionOffset;
    private byte instanceLimit;
    private ushort fadeInMS;
    private ushort fadeOutMS;
    private byte maxInstanceBehavior;
    private byte instanceCount;
}

internal struct FACTVariation
{
    //FAUDIONAMELESS union
    [StructLayout(LayoutKind.Explicit)]
    private struct NAMELESSUNION0
    {
        private struct _simple
        {
            private ushort track;
            private byte wavebank;
        }

        [FieldOffset(0)] private readonly _simple simple;
        [FieldOffset(0)] private readonly uint soundCode;
    }

    private NAMELESSUNION0 union;
    private float minWeight;
    private float maxWeight;
    private uint linger;
}

internal unsafe struct FACTVariationTable
{
    private byte flags;
    private short variable;
    private byte isComplex;

    private ushort entryCount;
    private FACTVariation* entries;
}

internal struct FACTTransition
{
    private int soundCode;
    private uint srcMarkerMin;
    private uint srcMarkerMax;
    private uint dstMarkerMin;
    private uint dstMarkerMax;
    private ushort fadeIn;
    private ushort fadeOut;
    private ushort flags;
}

internal unsafe struct FACTTransitionTable
{
    private uint entryCount;
    private FACTTransition* entries;
}

/* Internal WaveBank Types */

internal unsafe struct FACTSeekTable
{
    private uint entryCount;
    private uint* entries;
}

/* Internal Cue Types */

internal struct FACTInstanceRPCData
{
    private float rpcVolume;
    private float rpcPitch;
    private float rpcReverbSend;
    private float rpcFilterFreq;
    private float rpcFilterQFactor;
}

internal struct FACTEventInstance
{
    private uint timestamp;
    private ushort loopCount;

    private byte finished;

    //FAUDIONAMELESS union
    [StructLayout(LayoutKind.Explicit)]
    private struct NAMELESSUNION0
    {
        [FieldOffset(0)] private readonly float value;
        [FieldOffset(0)] private readonly uint valuei;
    }
}

internal unsafe struct FACTTrackInstance
{
    /* Tracks which events have fired */
    public FACTEventInstance* events;

    /* RPC instance data */
    public FACTInstanceRPCData rpcData;

    /* SetPitch/SetVolume data */
    public float evtPitch;
    public float evtVolume;

    /* Wave playback */
    public struct _wave
    {
        public FACTWave* wave;
        public float baseVolume;
        public short basePitch;
        public float baseQFactor;
        public float baseFrequency;
    }

    public _wave activeWave, upcomingWave;
    public FACTEvent* waveEvt;
    public FACTEventInstance* waveEvtInst;
}

internal unsafe struct FACTSoundInstance
{
    /* Base Sound reference */
    public FACTSound* sound;

    /* Per-instance track information */
    public FACTTrackInstance* tracks;

    /* RPC instance data */
    public FACTInstanceRPCData rpcData;

    /* Fade data */
    public uint fadeStart;
    public ushort fadeTarget;
    public byte fadeType; /* In (1), Out (2), Release RPC (3) */

    /* Engine references */
    public FACTCue* parentCue;
}

/* Internal Wave Types */

internal unsafe struct FACTWaveCallback
{
    /*FAudioVoiceCallback*/
    private void* callback;
    private FACTWave* wave;
}

/* Public XACT Types */

internal unsafe struct FACTAudioEngine
{
    public uint refcount;

    /*FACTNotificationCallback*/
    public void* notificationCallback;

    /*FACTReadFileCallback*/
    public void* pReadFile;

    /*FACTGetOverlappedResultCallback*/
    public void* pGetOverlappedResult;

    public ushort categoryCount;
    public ushort variableCount;
    public ushort rpcCount;
    public ushort dspPresetCount;
    public ushort dspParameterCount;

    public char** categoryNames;
    public char** variableNames;
    public uint* rpcCodes;
    public uint* dspPresetCodes;

    public FACTAudioCategory* categories;
    public FACTVariable* variables;
    public FACTRPC* rpcs;
    public FACTDSPPreset* dspPresets;

    /* Engine references */
    public LinkedList* sbList;
    public LinkedList* wbList;
    public FAudioMutex sbLock;
    public FAudioMutex wbLock;
    public float* globalVariableValues;

    /* FAudio references */
    public FAudio* audio;
    public FAudioMasteringVoice* master;
    public FAudioSubmixVoice* reverbVoice;

    /* Engine thread */
    public FAudioThread apiThread;
    public FAudioMutex apiLock;
    public byte initialized;

    /* Allocator callbacks */
    /*FAudioMallocFunc*/
    public void* pMalloc;

    /*FAudioFreeFunc*/
    public void* pFree;

    /*FAudioReallocFunc*/
    public void* pRealloc;

    /* Peristent Notifications */
    public FACTNoticationsFlags notifications;
    public void* cue_context;
    public void* sb_context;
    public void* wb_context;
    public void* wave_context;
    public LinkedList* wb_notifications_list;

    /* Settings handle */
    public void* settings;
}

internal unsafe struct FACTSoundBank
{
    /* Engine references */
    public FACTAudioEngine* parentEngine;
    public FACTCue* cueList;
    public byte notifyOnDestroy;
    public void* usercontext;

    /* Array sizes */
    public ushort cueCount;
    public byte wavebankCount;
    public ushort soundCount;
    public ushort variationCount;
    public ushort transitionCount;

    /* Strings, strings everywhere! */
    public char** wavebankNames;
    public char** cueNames;

    /* Actual SoundBank information */
    public char* name;
    public FACTCueData* cues;
    public FACTSound* sounds;
    public uint* soundCodes;
    public FACTVariationTable* variations;
    public uint* variationCodes;
    public FACTTransitionTable* transitions;
    public uint* transitionCodes;
}

internal unsafe struct FACTWaveBank
{
    /* Engine references */
    private FACTAudioEngine* parentEngine;
    private LinkedList* waveList;
    private FAudioMutex waveLock;
    private byte notifyOnDestroy;
    private void* usercontext;

    /* Actual WaveBank information */
    private char* name;

    private uint entryCount;

    /*FACTWaveBankEntry*/
    private void* entries;
    private uint* entryRefs;
    private FACTSeekTable* seekTables;
    private char* waveBankNames;

    /* I/O information */
    private uint packetSize;
    private ushort streaming;
    private byte* packetBuffer;
    private uint packetBufferLen;
    private void* io;
}

internal unsafe struct FACTWave
{
    /* Engine references */
    public FACTWaveBank* parentBank;
    public FACTCue* parentCue;
    public ushort index;
    public byte notifyOnDestroy;
    public void* usercontext;

    /* Playback */
    public uint state;
    public float volume;
    public short pitch;
    public byte loopCount;

    /* Stream data */
    public uint streamSize;
    public uint streamOffset;
    public byte* streamCache;

    /* FAudio references */
    public ushort srcChannels;
    public FAudioSourceVoice* voice;
    public FACTWaveCallback callback;
}

[StructLayout(LayoutKind.Sequential)]
internal unsafe struct FACTCue
{
    /* Engine references */
    public FACTSoundBank* parentBank;
    public FACTCue* next;
    public byte managed;
    public ushort index;
    public byte notifyOnDestroy;
    public void* usercontext;

    /* Sound data */
    public FACTCueData* data;

    //FAUDIONAMELESS union
    [StructLayout(LayoutKind.Explicit)]
    public struct NAMELESSUNION0
    {
        [FieldOffset(0)] public FACTVariationTable* variation;

        /* This is only used in scenarios where there is only one
		 * Sound; XACT does not generate variation tables for
		 * Cues with only one Sound.
		 */
        [FieldOffset(0)] public FACTSound* sound;
    }

    public NAMELESSUNION0 union0;

    /* Instance data */
    public float* variableValues;
    public float interactive;

    /* Playback */
    public uint state;
    public FACTWave* simpleWave;
    public FACTSoundInstance* playingSound;
    public FACTVariation* playingVariation;
    public uint maxRpcReleaseTime;

    /* 3D Data */
    public byte active3D;
    public uint srcChannels;

    public uint dstChannels;

    //public fixed float matrixCoefficients[2 * 8]; /* Stereo input, 7.1 output */
    public Matrix matrixCoefficients; // matrix is 16 floats

    /* Timer */
    public uint start;
    public uint elapsed;
}
