import { IShow } from "./show";

export interface IEnvelope
{
    shows: IShow[];
    showsCount: number;
}