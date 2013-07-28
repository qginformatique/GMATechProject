#!/bin/bash
rm -rf Blog
#git clone --depth 1 https://github.com/qginformatique/GMATechProject.git
git clone git@github.com:qginformatique/GMATechProject.git
chmod +x Blog/Build-GMATechProject-Mono.sh
./UpdateGMATechProject.sh
