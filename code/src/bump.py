'''
Created on Nov 2, 2012

@author: Geoff
'''
import tempfile
import shutil
import os
import re

def writeVersionToFile(filePath, major, minor, build, revision):
    fh, abs_path = tempfile.mkstemp()
    
    print 'Updating file {0} to version {1}.{2}.{3}.{4}'.format(filePath, major, minor, build, revision)
    new_file = open(abs_path, 'wb')
    old_file = open(filePath, 'rb')
    for line in old_file:
        if re.match(r'\[assembly: AssemblyFileVersion\("\d+\.\d+\.(\d+|\*)(\.(\d+|\*))?"\)\]', line):
            line = '[assembly: AssemblyFileVersion("{0}.{1}.{2}.{3}")]\r\n'.format(major, minor, build, revision)
        if re.match(r'\[assembly: AssemblyVersion\("\d+\.\d+\.(\d+|\*)(\.(\d+|\*))?"\)\]', line):
            line = '[assembly: AssemblyVersion("{0}.{1}.{2}.{3}")]\r\n'.format(major, minor, build, revision)

        new_file.write(line)
       
    old_file.close()
    new_file.close()
    os.close(fh)
    
    os.remove(filePath)
    shutil.move(abs_path, filePath)

def bumpVersion(args):
    version = []
    for idx in range(4):
        if idx < len(args):
            version.append(args[idx])
        else:
            version.append(0)

    #Creating a closure that is returned to remove globals
    def updateFile(filePath):
        major, minor, build, revision = version
        writeVersionToFile(filePath, major, minor, build, revision)
        
    return updateFile 

def validateVersion(version):
    if(len(version) == 0 or len(version) > 4):
        raise "invalid version must be at least one number"
    
    for itm in version:
        if(not itm.isdigit() and itm != "*"):
            err = itm + " is not a valid version segment"
            raise Exception(err)

    

if __name__ == '__main__':
    import sys
    import walklevel
    import fnmatch

    version = ["1", "0", "0", "0"]
        
    if len(sys.argv) > 1:
        arg = sys.argv[1].split('.')
        
        for i in range(0,4):
            if(len(arg) > i):
                version[i] = arg[i]

        validateVersion(version)
        for root, dirNames, filenames in walklevel.walklevel(".\\", level = 2):
            for filename in fnmatch.filter(filenames, "AssemblyInfo.cs"):
                bumpVersion(version)(os.path.join(root,filename))
    else:
        print('Usage: bump.py major[.minor[.revision[.build]]]')
        