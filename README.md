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
