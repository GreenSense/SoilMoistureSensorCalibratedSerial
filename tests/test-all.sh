
echo ""
echo "========================================"
echo "Starting test suite..."
echo ""
bash test-basic-calibration.sh && \
bash test-reverse-calibration.sh && \
echo "" && \
echo "All tests completed successfully!"
echo "========================================"
