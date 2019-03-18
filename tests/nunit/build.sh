echo "Building project tests"
echo "Dir: $PWD"

xbuild src/SoilMoistureSensorCalibratedSerial.sln /p:Configuration=Release /verbosity:quiet && \

echo "Finished building project tests." ||

(echo "Failed building project tests!" && exit 1)
