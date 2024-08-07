#!/bin/bash

archive=''

# TODO: install and mv the main script

curl -OLs https://github.com/FelipeMCassiano/nubemu/releases/lastest/download/$archive

tar -xzf $archive

echo `mv`

rm $archive
