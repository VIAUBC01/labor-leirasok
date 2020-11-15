import { Tweet, TweetWithId } from "./models";

import { v4 as uuidv4 } from 'uuid';

function generateTweetId() : string {
    return uuidv4(); 
}

export class Database {
    private tweetsByIds: Map<string, Tweet> = new Map();

    public addTweet(t: Tweet): string {
        let id = generateTweetId();
        this.tweetsByIds.set(id, t);
        return id;
    }

    public getTweetById(id: string): TweetWithId | null {
        if (!this.tweetsByIds.has(id))
            return null;
        return {
            ...this.tweetsByIds.get(id)!,
            id
        };
    }

    public getAllTweets() : TweetWithId[] {
        return Array.from(this.tweetsByIds.keys()).map(id => ({
            id,
            ...this.tweetsByIds.get(id)!
        }));
    }

    public deleteTweets(ids: string[]) : number {
        if (!ids)
            return 0;
        let existingIds = ids.filter(id => this.tweetsByIds.has(id));
        for (let id of existingIds)
            this.tweetsByIds.delete(id);
        
        return existingIds.length;
    }

    public searchTweet(text: string, tags: string[] | null) : TweetWithId[] {
        let lower = (text || '').toLowerCase();
        let list : TweetWithId[] = [];
        for (let id of Array.from(this.tweetsByIds.keys())) {
            let tweet = this.tweetsByIds.get(id)!;
            if (text && tweet.text.toLowerCase().indexOf(lower) < 0)
                continue;
            if (tags && !(tweet.tags|| []).some(tag => tags.includes(tag)))
                continue;
            list.push({
                ...tweet,
                id
            });
        }
        return list;
    }

}

