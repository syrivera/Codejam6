import { useEffect, useState } from 'react';
import './App.css';
import AddMeal from './Pages/AddMeal';
import SearchMeal from './Pages/SearchMeal';
import ProgressView from './Pages/ProgressView';

function App() {

    const [meals, setMeals] = useState();

    // App is simple and cleanly structured - easy to understand the components
    return (
        <div>
            <h1 id="tableLabel">🍽️Meal Tracker</h1>
            <p>This is a really sick webapp where you can log, track, and manage your meals!</p>
            <div>
            <div className="components-container">
                <div className="left-column">
                    <AddMeal></AddMeal>
                </div>
                <div className="right-column">
                    <SearchMeal></SearchMeal>
                    <ProgressView></ProgressView>
                </div>
            </div>
        </div>
        </div>
    );
    
}

export default App;