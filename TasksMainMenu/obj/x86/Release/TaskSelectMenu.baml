   M S B A M L   `   `   `  ����   TasksMainMenuD2clr-namespace:TasksMainMenu;assembly=TasksMainMenuTasksMainMenu  O Kmscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e0891&clr-namespace:System;assembly=mscorlibSystem "    TasksMainMenu.TaskSelectMenu   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     Dlocal2clr-namespace:TasksMainMenu;assembly=TasksMainMenu     6sys&clr-namespace:System;assembly=mscorlib     -     � Title$$  Functional Task Selection Menu��$	��720��$
��1280�� � WindowState$ 	Maximized=� � WindowStyle$
 None=� � Topmost .  .��� -   $��taskSelectGrid�� � ShowGridLines .  ����  �� ImageSource$$ /Resources/blankbackground.png�� j� 
AlignmentX$ Center=� j� 
AlignmentY$	 Top=� j� Stretch$ Uniform=�	 � RowDefinitions	 �� $
��121* ��� $	��28* ��� $
��132* �
 � ColumnDefinitions
 �� $	��35* ��� $
��105* ��� $
��136* ��� $	��60* ��� $	��60* ��� $	��72* ��� $	��35* �.+��� -   $��stackPanel1��$
��Auto��$��Right=�$��0,12,0,0q�$	��Top=�$
��Auto�� �� Orientation$ Vertical=� � UseLayoutRounding .   � ForceCursor . $��6�� � 	Resources  �� 
TargetType �� Property �� Value��%3   '��       �� " ��.���  � ��$ 0,10,0,0q��� -   $��instructionsTextBlk��$
��auto��$
��Bold"�$s��mSelect your task for participant # {0}, time-point {1} from the dropbox below and click "Continue"vbgfhgfhgfh��$
��Wrap=�$��60(� .   � OverridesDefaultStyle . $��Stretch=�$��Stretch=� J� IsHitTestVisible . $��1��$��5���� -   $��backToMainMenuBtn��$��Back to Main Menu��$��71��$
��Left=�$��	20,0,0,12q�$��Bottom=�$
��Auto��$��48(�$��2��$��3���� -   $��	comboBox1��$��48(�$��1��$��2��$��3��$��Center=� ��%G   (��       *,*SystemParameters.VerticalScrollBarWidthKey+)X�100Q�.@��� $	��ATM��$��42(��� $��
DoctorTest��$��42(��� $��Prescription Refill��$��42(��� $��Forms��$��42(��� $��Reaction Test��$��42(��� $��MetroTickets��$��42(��� $��OnlineBanking��$��42(��� -   $��button1��$��Continue��$��48(�$��3��$��67��$��Right=�$��	0,0,20,12q�$��Bottom=�$
��Auto��$��4��$��2��