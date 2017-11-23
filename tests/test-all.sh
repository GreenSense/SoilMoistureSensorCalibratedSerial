
echo ""
echo "========================================"
echo "Starting test suite..."
echo ""
bash test-basic-calibration.sh && \
bash test-reverse-calibration.sh && \
echo "" && \
echo ""

if [ $? -eq 0 ]; then
  echo "All tests completed successfully!"
else
  echo "Tests failed"
  exit 1
fi

echo "========================================"
