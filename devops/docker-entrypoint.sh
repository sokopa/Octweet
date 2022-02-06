#!/bin/sh

# Abort on any error (including if wait-for-it fails).
set -e

# Wait for the backend to be up, if we know where it is.
if [ -n "$DB_HOST" ]; then
  /app/devops/wait-for-it.sh -t 30 "$DB_HOST:${DB_PORT:-3306}"
fi

# Run the main container command.
exec "$@"