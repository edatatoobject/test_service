# Description:
TestService project provides some small and easy test for users. All test must contain a few steps with different provided data.


# Project requirements:
All tests must be generic. Contain a different numbers of steps. 
All tests, tests data and then answers collect in PostgreSql database.
Service must provide Api part. As user identifier must be used generated session id.
After finish test, service must provide number of correct answers, and percentage between of correct answers and how many users answer same.

# API part:
Project provides api for integration tests to other sites.
First call must get test id and user name, then initialize test and generate session id.

# Buisness logic:
All intermediate data must contains in Redis cache. After first call user name and test id must be saved with sessionId key.
All answers saving with key pattern `"sessionId[stepNumber]"`. Cache must have 1 hour of lifespan.
After finish caches saved through test must be removed.

# Development
Require use TDD for development. For testing use xUnit testing. User name for functions must be `xUnit`.
For unit tests use first existing service test.
Test must pass next unit tests:
1. Pass all steps, check added data in database and flushes cache in redis.

2.  Check test cancellation. After half of steps cancel the test, and check if cache for current session was flushed.
