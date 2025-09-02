import { Database } from "./database";
import express from 'express';
import bodyParser from 'body-parser';
import { Tweet } from "./models";

export class TwitterApi {

    constructor(private db: Database, private port: number) {

    }

    public startServer() {
        const app: express.Application = express();

        app.use(function (req, res, next) {
            res.header("Access-Control-Allow-Origin", "*");
            res.header("Access-Control-Allow-Headers", "Origin, X-Requested-With, Content-Type, Accept");
            next();
        });
        app.use(bodyParser.json());

        this.configureRouting(app);

        app.listen(this.port, () => {
            console.log(`Twitter server listening on port ${this.port}!`);
        });

    }


    private configureRouting(app: express.Application) {
        app.get('/api/', (req, res) => {
            res.json({
                name: 'Twitter server'
            });
        });

        app.post('/api/tweets', (req, res) => {
            console.log(req.body);
            let tweet = req.body as Tweet;
            this.db.addTweet(tweet);
            res.sendStatus(204);
        });

        app.get('/api/tweets/search', (req, res)=> { //expected format: ?tags=...&text=...
            let text = req.query.text as string;
            let tags = req.query.tags as string; 
            let tagsArray = tags && tags.split(",") || null;
            let tweets = this.db.searchTweet(text, tagsArray);
            res.json(tweets);
        });

        app.get('/api/tweets/:id', (req, res) => {
            let id = req.params.id;
            let tweet = this.db.getTweetById(id);
            if (!tweet) {
                res.sendStatus(404);
            } else {
                res.json(tweet);
            }
        });

        app.get('/api/tweets', (req, res) => {
            res.json(this.db.getAllTweets());
        });

        
        app.delete('/api/tweets/:ids', (req, res) => {
            let ids = req.params.ids;
            let idList = (ids || '').split(",");
            let count = this.db.deleteTweets(idList);
            res.json({
                deleted: count
            });
        });

       
    }


}