export interface User{
    username: string;
    token: string;
}

let data : number | string= 42;

data = 10;

interface Car{
    color: string;
    model: string;
    topSpeed?: number;
}

const char1: Car= {
    color:'blue',
    model: 'BMW'
}

const char2: Car = {
    color : 'red',
    model: 'Mercedes',
    topSpeed: 100
}

const multiply = (x: number,y:number)=> {
    return x*y;
}