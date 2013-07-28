#!/bin/bash

# ask supervisor to stop the production environment
supervisorctl stop gmatechproject

# clean production environment
rm /srv/gmatechproject/bin -rf
rm /srv/gmatechproject/Scripts -rf
rm /srv/gmatechproject/Resources -rf
rm /srv/gmatechproject/Views -rf

# Copy gmatechproject_test to gmatechproject
cp -r /srv/gmatechproject_test/bin /srv/gmatechproject/bin
cp -r /srv/gmatechproject_test/Scripts /srv/gmatechproject/Scripts
cp -r /srv/gmatechproject_test/Resources /srv/gmatechproject/Resources
cp -r /srv/gmatechproject_test/Views /srv/gmatechproject/Views

# ask supervisor to start the production environment
supervisorctl start gmatechproject