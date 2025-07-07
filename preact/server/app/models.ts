export type Tweet = {
    text : string,
    userName: string,
    tags?: string[]
}

export type TweetWithId = Tweet & {
    id?: string
}