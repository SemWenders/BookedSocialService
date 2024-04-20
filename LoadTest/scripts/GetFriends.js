import http, { get } from "k6/http";
import { check, sleep } from "k6";

const isNumeric = (value) => /^\d+$/.test(value);

const default_vus = 10;

const target_vus_env = `${__ENV.TARGET_VUS}`;
const host_url = __ENV.HOST_URL || "http://host.docker.internal:5166";
const target_vus = isNumeric(target_vus_env)
  ? Number(target_vus_env)
  : default_vus;

export let options = {
  insecureSkipTLSVerify: true,
  scenarios: {
    getFriends: {
      executor: "constant-vus",
      duration: "20s",
      exec: "getFriends",
      vus: 10,
    },
    sendFriendRequest: {
      executor: "constant-vus",
      duration: "20s",
      exec: "sendFriendRequest",
      vus: 10,
      startTime: "20s",
    },
  },
};

export function getFriends() {
  const response = http.get(`${host_url}/Social/GetFriends/1`, {
    headers: { Accepts: "application/json" },
  });
  console.log(response);
  check(response, { "status is 200": (r) => r.status === 200 });
  sleep(0.3);
}

export function sendFriendRequest() {
  const response = http.get(`${host_url}/Social/SendFriendRequest/2/3`, {
    headers: { Accepts: "application/json" },
  });
  check(response, { "status is 200": (r) => r.status === 200 });
  sleep(0.3);
}
