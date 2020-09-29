import { IEpisodes } from "./episodes";

export interface ISeason
{
    seasonNumber: number;
    episodes: IEpisodes[];
}