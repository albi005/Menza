#!/usr/bin/env bash

set -e

PrevDir=$PWD
cd "$(dirname "$0")" # cd to the directory of the script

docker build --target final -t menza -f Dockerfile .
mkdir -p ./bin
docker save menza -o ./bin/menza.tar
tmp=$(ssh albi@iron 'mktemp -d')
echo $tmp
rsync --progress --compress bin/menza.tar albi@iron:$tmp
ssh root@iron "docker load -i $tmp/menza.tar"
ssh root@iron 'systemctl restart docker-menza.service'

cd $PrevDir
