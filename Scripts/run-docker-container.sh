#!/bin/bash
docker stop website
docker rm website
sudo docker run -v /home/pi/certs/:/certs/ -e ASPNETCORE_ENVIRONMENT=Production --name website -d -p 80:80 -p 443:443 website:latest