import { ConservationStatus } from "./ConservationStatus";

export interface BirdDetailViewModel {
    birdId: number;
    class: string;
    order: string;
    family: string;
    genus: string;
    species: string;
    englishName: string;
    internationalName: string;
    category: string;
    populationSize: string;
    btoStatusInBritain: string;
    thumbnailUrl: string;
    // songUrl: string;
    creationDate: Date | string;
    lastUpdateDate: Date | string;
    birdConservationStatus: ConservationStatus;
    birderStatus: string;
}
