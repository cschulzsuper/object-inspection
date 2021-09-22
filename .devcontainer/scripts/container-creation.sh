set -e

# Install version 6.0 of dotnet
./eng/dotnet-install.sh -Channel 6.0.1xx -Quality GA -InstallDir ./.dotnet

# Download and install the emulator cert
curl -k https://cosmos:8081/_explorer/emulator.pem > ~/emulatorcert.crt
sudo cp ~/emulatorcert.crt /usr/local/share/ca-certificates/
sudo update-ca-certificates

# The container creation script is executed in a new Bash instance
# so we exit at the end to avoid the creation process lingering.
exit