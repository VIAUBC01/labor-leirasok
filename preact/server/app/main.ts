import { Database } from "./database";
import { TwitterApi } from "./web-api";

let db = new Database();

db.addTweet({
    text: 'Hello World!',
    tags: ["init"],
    userName: "mark"
});

let api = new TwitterApi(db, 3000);
api.startServer();



