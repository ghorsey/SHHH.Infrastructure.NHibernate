'''
Created on Nov 2, 2012

@author: Geoff
'''
import fnmatch
import os
import shutil
import walklevel

matches = []
nugetPath = ".\\.nuget\\nuget.exe"
outputPath = ".\\packages"

for root, dirNames, filenames in walklevel.walklevel(".\\", level = 2):
    for filename in fnmatch.filter(filenames, "packages.config"):
        matches.append(os.path.join(root, filename))

for package in matches:
    os.system(nugetPath + " install " + package + " -OutputDirectory " + outputPath)