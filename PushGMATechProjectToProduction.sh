#!/bin/bash

# ask supervisor to stop the production environment
supervisorctl stop gmatechproject

# clean production environment
rm /srv/gmatechproject/bin -rf
rm /srv/gmatechproject/Scripts -rf
rm /srv/gmatechproject/Resources -rf
rm /srv/gmatechproject/Views -rf

# ask supervisor to start the production environment
supervisorctl start gmatechproject
