#!/bin/bash
input="input.txt"
low=$(sed -E 's/(.*)(one|two|three|four|five|six|seven|eight|nine|[1-9])(.*)/\2/' input.txt | sed -e 's/one/1/g' -e 's/two/2/g' -e 's/three/3/g' -e 's/four/4/g' -e 's/five/5/g' -e 's/six/6/g' -e 's/seven/7/g' -e 's/eight/8/g' -e 's/nine/9/g' | awk '{s+=$1} END {printf "%.0f\n", s}')
high=$(awk 'match($0, /(one|two|three|four|five|six|seven|eight|nine|[1-9])/) { print substr($0, RSTART, RLENGTH) }' input.txt | sed -e 's/one/1/g' -e 's/two/2/g' -e 's/three/3/g' -e 's/four/4/g' -e 's/five/5/g' -e 's/six/6/g' -e 's/seven/7/g' -e 's/eight/8/g' -e 's/nine/9/g' | awk '{s+=$1} END {printf "%.0f\n", s}')
total=$((low + high * 10))
echo "$total"