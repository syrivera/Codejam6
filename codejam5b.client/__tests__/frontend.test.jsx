import React from 'react';
import { render, screen, fireEvent, waitFor, act } from '@testing-library/react';
import SearchMeal from '../src/Pages/SearchMeal';
import ProgressView from '../src/Pages/ProgressView';
//test file
const mockFetch = (data, shouldFail = false) => {
  if (shouldFail) {
    return jest.fn(() => Promise.reject(new Error('API Error')));
  }
  return jest.fn(() => 
    Promise.resolve({ 
      ok: true, 
      json: () => Promise.resolve(data) 
    })
  );
};
// test for searching meals 
describe('SearchMeal Component', () => {
  afterEach(() => {
    jest.restoreAllMocks();
  });

  test('renders search input and button', () => {
    render(<SearchMeal />);
    
    const searchInput = screen.getByPlaceholderText(/enter meal name/i);
    const searchButton = screen.getByRole('button', { name: /search/i });
    
    expect(searchInput).toBeInTheDocument();
    expect(searchButton).toBeInTheDocument();
    expect(searchInput).toHaveValue('');
  });

  test('searches meals and displays results', async () => {
    const mockMeals = [
      { id: 1, name: 'Chicken Caesar Salad', calories: 450, carbs: 30, fat: 25, protein: 35 },
      { id: 2, name: 'Grilled Chicken Wrap', calories: 380, carbs: 45, fat: 12, protein: 28 }
    ];
    
    global.fetch = mockFetch(mockMeals);

    render(<SearchMeal />);
    
    const searchInput = screen.getByPlaceholderText(/enter meal name/i);
    const searchButton = screen.getByRole('button', { name: /search/i });
    
    fireEvent.change(searchInput, { target: { value: 'chicken' } });
    fireEvent.click(searchButton);
    
    expect(global.fetch).toHaveBeenCalledWith('/api/meals/search?name=chicken');
    
    await waitFor(() => {
      expect(screen.getByText('Chicken Caesar Salad')).toBeInTheDocument();
      expect(screen.getByText('Grilled Chicken Wrap')).toBeInTheDocument();
    });
  });

  test('displays empty state when no results found', async () => {
    global.fetch = mockFetch([]);

    render(<SearchMeal />);
    
    const searchInput = screen.getByPlaceholderText(/enter meal name/i);
    const searchButton = screen.getByRole('button', { name: /search/i });
    
    fireEvent.change(searchInput, { target: { value: 'xyz123' } });
    fireEvent.click(searchButton);
    
    await waitFor(() => {
      expect(screen.queryByText('Chicken')).not.toBeInTheDocument();
    });
  });
});

describe('ProgressView Component', () => {
  beforeEach(() => {
    jest.useFakeTimers();
  });

  afterEach(() => {
    act(() => {
      jest.runOnlyPendingTimers();
    });
    jest.useRealTimers();
    jest.restoreAllMocks();
  });
//test for progress view 
  test('displays loading state', () => {
    global.fetch = jest.fn(() => new Promise(() => {}));

    render(<ProgressView />);
    
    expect(screen.getByText(/loading/i)).toBeInTheDocument();
  });

  test('fetches and displays all progress data', async () => {
    const mockProgressData = {
      currentWeight: 185,
      targetWeight: 165,
      targetDailyCalories: 2100,
      targetDailyCarbs: 240,
      targetDailyFat: 70,
      targetDailyProtein: 150,
      consumedCalories: 800,
      consumedCarbs: 90,
      consumedFat: 25,
      consumedProtein: 60
    };
    
    global.fetch = mockFetch(mockProgressData);

    render(<ProgressView />);
    
    expect(global.fetch).toHaveBeenCalledWith('/api/progress');
    
    await waitFor(() => {
      expect(screen.getByText('185 lbs')).toBeInTheDocument();
      expect(screen.getByText('165 lbs')).toBeInTheDocument();
      expect(screen.getByText('2100 kcal')).toBeInTheDocument();
    });
    
    expect(screen.getByText('800')).toBeInTheDocument();
    expect(screen.getByText('90')).toBeInTheDocument();
    expect(screen.getByText('25')).toBeInTheDocument();
    expect(screen.getByText('60')).toBeInTheDocument();
  });

  test('displays error message on failed fetch', async () => {
    global.fetch = mockFetch(null, true);

    render(<ProgressView />);
    
    await waitFor(() => {
      expect(screen.getByText(/error/i)).toBeInTheDocument();
    });
  });
});
