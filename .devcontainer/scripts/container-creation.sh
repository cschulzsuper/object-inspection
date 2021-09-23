set -e

# Install version 6.0 of locally
./eng/dotnet-install.sh -Channel 6.0.1xx -Quality GA -InstallDir ./.dotnet

# Wait for the emulator to have started
DIR="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
"$DIR/wait-for-it.sh" cosmos:8081

# Download and install the emulator cert
curl -k https://cosmos:8081/_explorer/emulator.pem > ~/emulatorcert.crt
sudo cp ~/emulatorcert.crt /usr/local/share/ca-certificates/
sudo update-ca-certificates

# Add .NET Dev Certs to environment to facilitate debugging.
# Do **NOT** do this in a public base image as all images inheriting
# from the base image would inherit these dev certs as well. 
dotnet dev-certs https

# The container creation script is executed in a new Bash instance
# so we exit at the end to avoid the creation process lingering.
exit