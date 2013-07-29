#!/bin/bash

# ask supervisor to stop the test environment
supervisorctl stop gmatechproject_test

cd GMATechProject
# Cancel any local changes to the git repository
git stash save --keep-index
git stash drop

# Update the git repository
git pull

# clean the test environment
rm /srv/gmatechproject_test/bin -rf
rm /srv/gmatechproject_test/Scripts -rf
rm /srv/gmatechproject_test/Resources -rf
rm /srv/gmatechproject_test/Views -rf

# execute the build script 
sh Build-GMATechProject-Mono.sh /srv/gmatechproject_test

# ask supervisor to start the test environment
supervisorctl start gmatechproject_test