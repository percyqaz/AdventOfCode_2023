#!/bin/bash
input="input.txt"
total=0
while IFS= read -r line
do
   numbers=$(echo "$line" | tr -d [:alpha:])
   number="${numbers:0:1}${numbers: -1}"
   total=$((total + number))
done < "$input"
echo "$total"