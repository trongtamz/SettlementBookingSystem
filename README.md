# SettlementBookingTest

## Versions

1. Basic Implementation

`BasicBookingController` provides an implementation within the controller that is complete but poorly written.

2. Mediator Implementation

`MediatorBookingController` provides an implementation using the mediator pattern and the data is stored in an [Interval Tree](https://en.wikipedia.org/wiki/Interval_tree) for O(log m + n) access.
This implementatrion also provides unit tests with a mocked repository.

## How to execute

Just run the `SettlementBookingSystem` project and make a POST request:

```
curl --location 'https://localhost:5001/booking' \
--header 'Content-Type: application/json' \
--data '{
    "name": "Keith",
    "bookingTime": "09:00"
}'
```
