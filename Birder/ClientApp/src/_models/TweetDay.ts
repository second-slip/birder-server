import { BirdSummaryViewModel } from './BirdSummaryViewModel';

export interface TweetDay {
    tweetDayId: number;
    displayDay: Date | string;
    creationDate: Date | string;
    lastUpdateDate: Date | string;
    bird: BirdSummaryViewModel;
}
