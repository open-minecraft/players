#!/bin/bash

dotnet publish ../src -r win-x64 -c Release /p:DebugType=None /p:DebugSymbols=false
