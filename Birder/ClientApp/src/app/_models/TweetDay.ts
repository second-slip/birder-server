import { BirdSummaryViewModel } from './BirdSummaryViewModel';

export interface TweetDay {
    tweetDayId: number;
    songUrl: string;
    displayDay: Date | string;
    creationDate: Date | string;
    lastUpdateDate: Date | string;
    bird: BirdSummaryViewModel;
}

export interface TweetArchiveDto {
    totalItems: number;
    items: TweetDay[];
}
