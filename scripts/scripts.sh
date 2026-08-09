#!/bin/bash

THIS_FILE_DIRECTORY=$(dirname "$0")

echo initializing scripts, args: "$0" "$@"
echo current directory = "$THIS_FILE_DIRECTORY"

realpath "$0"
echo "${BASH_SOURCE[0]}"

source "$THIS_FILE_DIRECTORY"/solution-scripts.sh