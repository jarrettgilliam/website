#!/bin/bash

docker buildx build --platform linux/amd64,linux/arm64 --push \
  -t jarrettgilliam/website:$(git rev-parse --short HEAD) \
  -t jarrettgilliam/website:latest \
  "$(dirname "$0")/.."