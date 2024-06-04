import sys

unityProjectSettingsPath = sys.argv[1];
buildDataPath = sys.argv[2];
bundleParameter = "bundleVersion: ";

with open(unityProjectSettingsPath) as file:
    projectVersion = file.readline();

projectVersion = projectVersion.split('.')

open(buildDataPath, 'a+')
    

with open(buildDataPath, 'r+') as buildDataFile:
    buildVersions = [x.strip().split('.') for x in buildDataFile.readlines()]

if (len(buildVersions) == 0):
    with open(buildDataPath, 'w+') as buildDataFile:
        version = projectVersion[0] + '.' + projectVersion[1] + '.0'
        buildDataFile.write(version)
        print(version)
else:
    lastVersions = next(filter(lambda x: x[0] == projectVersion[0] and x[1] == projectVersion[1], buildVersions), None)
    if (lastVersions == None):
        lastVersions = projectVersion.copy()
        buildVersions.append(lastVersions)
        print('.'.join(lastVersions))
    else:
        lastVersions[2] = str(int(lastVersions[2]) + 1)
        version = lastVersions[0] + '.' + lastVersions[1] + '.' + lastVersions[2]
        print(version)
    with open(buildDataPath, 'w+') as buildDataFile:
        buildDataFile.write('\n'.join(['.'.join(x) for x in buildVersions]))