echo Building FibreSud

# Build the solution with the MonoRelease configuration
xbuild /property:Configuration=MonoRelease

# Create the build directory if needed
mkdir -p build

# Empty the build directory
rm -R -f build/*

# Copy the build to the build directory
cp -R GMATechProject.Web/Resources build/
cp -R GMATechProject.Web/Scripts build/
cp -R GMATechProject.Web/Upload build/
cp -R GMATechProject.Web/Views build/
cp -R GMATechProject.Web/bin build/
mkdir -p build/logs

# If we have arguments passed to the script
if [ $# -gt 0 ]; then	
	echo Build Finished: copy to $1
	# If the specified output directory doesn't exist
	if [ ! -d $1 ]; then
		# Create it
		sudo mkdir $1
	fi

	# Copy the build to the output directory
	sudo cp -R build/* $1

# If we don't have arguments passed to the script
else
	echo Build Finished
fi
