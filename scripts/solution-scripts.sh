#!/bin/bash

function remove_from_solution() {
    solution="$1"
    path="$2"
    
    while IFS= read -r file
    do
      echo removing "$file" from "$solution"
      dotnet solution "$solution" remove "$file"
    done <<< "$(find "$path" -name '*.csproj')"
}