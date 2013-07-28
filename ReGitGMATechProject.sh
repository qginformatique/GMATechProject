#!/bin/bash
rm -rf GMATechProject
#git clone --depth 1 https://github.com/qginformatique/GMATechProject.git
git clone git@github.com:qginformatique/GMATechProject.git
chmod +x GMATechProject/Build-GMATechProject-Mono.sh
./UpdateGMATechProject.sh
