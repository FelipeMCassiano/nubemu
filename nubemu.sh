#!/bin/bash

localstack start

docker-compose up -d

echo '$nubemu running at localhost:2207'
