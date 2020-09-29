import { ICharacter } from "./character";
import { IPerson } from "./person";

export interface IActor
{
    person: IPerson;
    character: ICharacter;
}