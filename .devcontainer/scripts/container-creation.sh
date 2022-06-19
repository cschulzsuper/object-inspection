set -e

# Install version 7.0 of locally
./eng/dotnet-install.sh -Channel 7.0.1xx -Quality preview -InstallDir ./.dotnet

# Add .NET Dev Certs to environment to facilitate debugging.
# Do **NOT** do this in a public base image as all images inheriting
# from the base image would inherit these dev certs as well. 
dotnet dev-certs https --trust

# The container creation script is executed in a new Bash instance
# so we exit at the end to avoid the creation process lingering.
exit