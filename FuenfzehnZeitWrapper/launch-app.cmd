@echo off

echo Starting Dev Tunnel 
start cmd /k devtunnel host

echo Starting Docker 
docker compose up --build