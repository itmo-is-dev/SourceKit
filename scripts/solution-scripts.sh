#!/bin/bash

function remove_from_solution() {
    solution="$1"
    path="$2"
    
    while IFS= read -r file
    do
      dotnet solution "$solution" remove "$file"
    done <<< "$(find "$path" -name '*.csproj')"
}