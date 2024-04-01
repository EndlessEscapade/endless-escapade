#!/bin/bash

compiler="../Assets/Effects/Compiler/fxc"

if [ ! -x "$compiler" ]; then
    echo "Expected compiler at: '$compiler'"
    exit 1
fi

effects="../Assets/Effects"

if [ ! -d "$effects" ]; then
    echo "Expected effects at: '$effects'."
    exit 1
fi

for shader in "$effects"/*.fx; do
    input="$shader"
    output="${input%.*}.fxc"

    "$compiler" /T fx_2_0 "$input_file" /Fo "$output_file"
    
    echo "Shader output successfully: $input -> $output"
done
