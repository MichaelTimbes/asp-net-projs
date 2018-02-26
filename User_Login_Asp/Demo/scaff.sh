#!/bin/bash
dotnet restore;
dotnet aspnet-codegenerator controller -name FriendController -m FriendModel -dc FriendContext --relativeFolderPath Controllers --useDefaultLayout --referenceScriptLibraries;