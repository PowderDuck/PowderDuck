wget $1 2>&1 | if [[ $(grep ERROR) ]]; then wget --no-check-certificate $1; fi
