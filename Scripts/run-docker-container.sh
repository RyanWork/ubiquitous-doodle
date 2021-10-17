#!/bin/bash
docker stop website
docker rm website
sudo docker run -v /home/pi/certs/ryanha-c7a5a7d4cc9b.json:/certs/ryanha-c7a5a7d4cc9b.json --name website -d -p 8080:80 website:latest