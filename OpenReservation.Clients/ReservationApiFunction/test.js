const assert = require("assert");
const { getApiPath, getQueryString } = require("./dist/index");

assert.strictEqual(getApiPath("/api/v1/notice"), "/api/v1/notices");
assert.strictEqual(getApiPath("/api/v1/reservation/123"), "/api/v1/reservations/123");
assert.strictEqual(
    getApiPath("/api/v1/reservationPlaces/123/periods"),
    "/api/v1/reservation-places/123/availability");
assert.strictEqual(
    getQueryString({ pageNumber: 2, keyword: "a b" }, "/api/v1/notices"),
    "?pageNumber=2&keyword=a%20b");
assert.strictEqual(
    getQueryString({ dt: "2026-11-02" }, "/api/v1/reservation-places/123/availability"),
    "?date=2026-11-02");
