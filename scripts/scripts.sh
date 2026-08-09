#!/bin/bash

THIS_FILE_DIRECTORY=$(dirname "${BASH_SOURCE[0]}")

echo initializing scripts, args: "$0" "$@"
echo current directory = "$THIS_FILE_DIRECTORY"

source "$THIS_FILE_DIRECTORY"/solution-scripts.sh