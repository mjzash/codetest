# Coding exercise

This implements a simple console solution to the coding test to validate and extract a zipfile.

### Requirements

Dotnet 8 is required for this and a simple `dotnet run` should get things going. `dotnet run <zipfile>` will allow you to select different zip files to check for invalid or corrupted zip files. It also respects some configuration information in `appsettings.json`. Alternatively, you can run this with Docker:

```
cd fileprocessor
docker build -t codetest -f Dockerfile .    
docker run -it -v $PWD:/Data/ --rm codetest /Data/<zipfile>
```

Make sure you change `zipfile` to the name of a zip file in the current directory. Three test zip files exist. `sample.zip` is the example file provided to me, `badextension.zip` is a zip file containing a file with an unrecognised extension and `missing-appno.zip` is a seemingly valid zip file, but the `party.XML` has an empty `applicationno` field.

For example, `docker run -it -v $PWD:/Data --rm codetest /Data/sample.zip`.

Alternatively, if you have your own zip file to test, say `baddata.zip`, you can use

```docker run -it -v $PWD:/Data/ --rm codetest /Data/baddata.zip```

### Some notes

- I avoided 3rd party dependencies for logging / compression / dependency injection as I felt this might complicate things unnecessarily
- I interpreted the instructions as meaning that a zipfile with files other than the accepted file extensions should be rejected
- I ran out of time for unit tests; probably the main complication of this would be mocking the `ZipArchive` as Microsoft hasn't provided an mockable interface for this.
- I didn't set up dependency injection although it could be done with autofac / Microsoft's implementation with a bit more work
- I added the docker file 'just in case' after the 3 hours expired.
- Tested on Windows and Linux